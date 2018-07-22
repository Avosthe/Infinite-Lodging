using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirusTotalNET;
using VirusTotalNET.ResponseCodes;
using VirusTotalNET.Results;

namespace SSDAssignment40.Data
{
    public class VirusScanner : IVirusScanner
    {
        public async Task<VirusReport> ScanForVirus(byte[] hostile)
        {
            VirusTotal virusTotal = new VirusTotal("1bab2d79f1076e459758fca49c07dd74ae912faf8de9e08d2569387c3cab968f"); // Maximum of 4 requests per minute for scrub api key. :(
            virusTotal.UseTLS = true;
            FileReport report = await virusTotal.GetFileReportAsync(hostile);
            return new VirusReport() { Positives = report.Positives, ReportLink = report.Permalink };
        }
    }
}
