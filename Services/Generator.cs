using System;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace NGODP.Services
{
    public class Generator : IGenerator
    {
        public Generator()
		{
		}

		public void Report(string DocUrl)
		{
			var doc = new HtmlToPdfDocument()
			{
				GlobalSettings = {
					ColorMode = ColorMode.Color,
					Orientation = Orientation.Portrait,
					PaperSize = PaperKind.A4,
					Margins = new MarginSettings() { Top = 10 },
					Out = @"D:\report.pdf",
				},
				Objects = {
					new ObjectSettings()
					{
						Page = DocUrl,
					},
				}
			};
		}

		public Guid GenerateGUID()
		{
			return Guid.NewGuid();
		}

		public string TimeStamp()
		{
			return DateTime.Now.ToLongTimeString();
		}

		public string FeedbackID()
		{
			return string.Concat("f",GeneratePartialID());
		}
		
		public string AdminID()
		{
			return string.Concat("a",GeneratePartialID());
		}

		public string StudentID()
		{
			return string.Concat("s",GeneratePartialID());
		}

		public string RequestID()
		{	
			return string.Concat("r",GeneratePartialID());
		}
		
		public string GeneratePartialID()
		{
			string yr = DateTime.Now.Year.ToString("####");
			string month = DateTime.Now.Month.ToString("##");
			string day = DateTime.Now.Day.ToString("##");
			string hr = DateTime.Now.Hour.ToString("##");
			string min = DateTime.Now.Minute.ToString("##");
			string sec = DateTime.Now.Second.ToString("##");
			string milli = DateTime.Now.Millisecond.ToString("##");
			
			return string.Concat(yr,month,day,hr,min,sec);
		}
    }
}
