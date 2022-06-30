using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrganizer.UI.Core {
    public class Mediator {
        public event Action<string> ShowMessage;
        public void SendMessage(string text) {
            ShowMessage?.Invoke(text);
        }
    }
}
