using CastAmNow.Sdk.Abstractions;
using Refit;

namespace CastAmNow.Sdk
{
    public class DefectApi(HttpClient defectClient) : IDefectApi
    {
        public IDefectService DefectService { get; } = RestService.For<IDefectService>(defectClient);
    }
}
