using System;

namespace NGODP.Services
{
    public interface IGenerator
    {
         Guid GenerateGUID();

         void Report(string DocUrl);

         string TimeStamp();

         string FeedbackID();

         string AdminID();

         string StudentID();

         string RequestID();
    }
}