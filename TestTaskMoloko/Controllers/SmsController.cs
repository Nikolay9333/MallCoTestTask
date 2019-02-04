using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTaskMoloko.Extensions;
using TestTaskMoloko.Helpers;
using TestTaskMoloko.Interfaces;
using TestTaskMoloko.Models;

namespace TestTaskMoloko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        #region  Fields

        private readonly ISmsSender _smsSender;
        private readonly DbContext _dbContext;

        #endregion

        #region  Controllers

        public SmsController(ISmsSender smsSender, DbContext dbContext)
        {
            _smsSender = smsSender;
            _dbContext = dbContext;
        }

        #endregion

        #region  Public Methods

        /// <summary>
        /// Выбирает из базы еще не отправленные сообщения и пробует отправить их через апи,
        /// если в ответе есть идентификатор сообщения и нет ошибки, то сообщение считается отправленным, иначе нет
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("send-messages")]
        public async Task<ActionResult<string>> TrySendMessages()
        {
            string content;

            try
            {
                var newMessages = _dbContext.Set<Message>().AsNoTracking().Where(m => !m.IsReceived).ToList();
                content = _smsSender.TrySendMessages(newMessages);

                if (TryParseContent(content, out var result) && string.IsNullOrEmpty(result.ErrCode))
                {
                    var saveMegsRespTask = _dbContext.Set<MessageResponse>()
                        .AddRangeAsync(result.Messages);

                    MarkMessagesAsReceived(result.Messages);
                    await Task.WhenAll(saveMegsRespTask);
                }
                else
                {
                    _dbContext.Set<CriticalError>().Add(new CriticalError(result.ErrCode));
                    _dbContext.SaveChanges();

                    return StatusCode(500);
                }
            }
            catch (Exception e)
            {
                var error = GetInnerErrorDescription(e);
                _dbContext.SaveChanges();

                return StatusCode(500, error);
            }

            _dbContext.SaveChanges();

            return content;
        }

        #endregion

        #region Private Methods

        private string GetBody()
        {
            string result;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        private object GetInnerErrorDescription(Exception e)
        {
            var message = default(string);
            while (e.InnerException != null)
            {
                message += e.Message;
                e = e.InnerException;
            }

            var errorObj = new {message, stackTrace = e.StackTrace}.Serialize();

            return errorObj;
        }

        private bool TryParseContent(string content, out PackageResponse packageResponse)
        {
            try
            {
                packageResponse = XmlHelper.Deserialize<PackageResponse>(content);
            }
            catch (Exception)
            {
                packageResponse = new PackageResponse()
                {
                    ErrCode = "Ошибка при парсинге ответа"
                };

                return false;
            }

            return true;
        }

        /// <summary>
        /// Получает количество полученных сообщений
        /// </summary>
        /// <param name="messages">Сообщения, полученные от апи</param>
        /// <returns>Возвращает количество полученных сообщений</returns>
        private int MarkMessagesAsReceived(List<MessageResponse> messages)
        {
            var count = 0;
            foreach (var messageResp in messages)
            {
                var curMsg = _dbContext.Set<Message>().Single(m => m.Id == messageResp.Id);
                if (!string.IsNullOrEmpty(messageResp.MsgErrCode)) continue;
                curMsg.IsReceived = true;
                ++count;
            }

            return count;
        }

        #endregion

    }
}