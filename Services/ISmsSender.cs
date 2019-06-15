using System;

namespace NGODP.Services
{
    public interface ISmsSender
    {
         object SendMessage(string Number, string Message, string Code);
    }
}