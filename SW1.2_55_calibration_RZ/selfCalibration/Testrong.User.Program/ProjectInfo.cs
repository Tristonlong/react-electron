
using System.Text;

namespace Testrong.User.Program
{
    public  class ProjectInfo
    {
        string _projectAuthor;
        string _projectTime;
        string _projectClient;
        string _projectDesCription;

        public string ProjectAuthor { get => _projectAuthor; set => _projectAuthor = value; }
        public string ProjectTime { get => _projectTime; set => _projectTime = value; }
        public string ProjectClient { get => _projectClient; set => _projectClient = value; }
        public string ProjectDesCription { get => _projectDesCription; set => _projectDesCription = value; }
        public string GetProjectInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ProjectAuthor: {_projectAuthor}");
            sb.AppendLine($"ProjectTime: {_projectTime}");
            sb.AppendLine($"ProjectClient: {_projectClient}");
            sb.AppendLine($"ProjectDesCription: {_projectDesCription}");
            return sb.ToString();
        }
    }
}
