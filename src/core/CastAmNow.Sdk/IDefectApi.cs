using CastAmNow.Sdk.Abstractions;

namespace CastAmNow.Sdk
{
    public interface IDefectApi
    {
        public IDefectService DefectService { get; }
    }
}
