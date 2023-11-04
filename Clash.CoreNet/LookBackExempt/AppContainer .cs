using System.ComponentModel;

namespace Clash.CoreNet.LookBackExempt;

public class AppContainer : INotifyPropertyChanged
{
    private string appContainerName1;
    private string displayName1;
    private string workingDirectory1;
    private string stringSid;
    private List<uint> capabilities1;
    private bool loopUtil;

    public string appContainerName { get => appContainerName1;set { appContainerName1 = value;OnPropertyChanged(nameof(appContainerName)); } }
    public string displayName { get => displayName1; set { displayName1 = value; OnPropertyChanged(nameof(displayName)); } }
    public string workingDirectory { get => workingDirectory1; set { workingDirectory1 = value; OnPropertyChanged(nameof(workingDirectory)); } }
    public string StringSid { get => stringSid; set { stringSid = value; OnPropertyChanged(nameof(StringSid)); } }
    public List<uint> capabilities { get => capabilities1; set { capabilities1 = value; OnPropertyChanged(nameof(capabilities)); } }
    public bool LoopUtil { get => loopUtil; set { loopUtil = value; OnPropertyChanged(nameof(LoopUtil)); } }

    public AppContainer(string _appContainerName, string _displayName, string _workingDirectory, IntPtr _sid)
    {
        appContainerName = _appContainerName;
        displayName = _displayName;
        workingDirectory = _workingDirectory;
        string tempSid;
        Toolkit.LoopUtil.ConvertSidToStringSid(_sid, out tempSid);
        StringSid = tempSid;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public event PropertyChangedEventHandler? PropertyChanged;
}
