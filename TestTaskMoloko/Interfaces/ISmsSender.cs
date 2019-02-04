using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskMoloko.Models;

namespace TestTaskMoloko.Interfaces
{
    public interface ISmsSender
    {
        string TrySendMessages(List<Message> messages);
    }
}
