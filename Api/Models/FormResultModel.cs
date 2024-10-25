namespace EccomercePage.Api.Models
{
    public class FormResultModel
    {
        public bool Succeeded { get; set; }
        public string[] ErrorList { get; set; } = [];
    }
}
