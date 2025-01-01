using static System_Utility.SD;

namespace Task_Mangement_Web.Models
{
    public class APIRequest
    {
        public APIType apiType {  get; set; } = APIType.GET;
        public string Url {  get; set; }
        public object Data {  get; set; }
        public string Token {  get; set; }
    }
}
