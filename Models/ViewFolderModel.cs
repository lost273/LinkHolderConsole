using System.Collections.Generic;

namespace LinkHolderConsole.Models {
    public class ViewFolderModel {
       public int Id { get; set; }
        public string Name { get; set; }
        public List<ViewLinkModel> MyLinks { get; set; }
    }
}