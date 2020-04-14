using System.Windows.Forms;

namespace ChatClient
{
    public class ReceivedFileInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ListViewItem LWItem { get; set; }
    }
}