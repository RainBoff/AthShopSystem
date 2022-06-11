using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2.Areas.MainAdminArea.Models
{
    public class ZamowieniaIndexViewModel
    {
       public IEnumerable<Data_Model_P.Zamowienie> zamowienia;
       public int IdSklepu;
    }
}
