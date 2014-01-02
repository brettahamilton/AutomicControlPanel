using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace AutomicControlPanel
{
    public class DemoServices
    {
        public int count_BaseServices { get; set; }
        public int count_OneServices { get; set; }
        public int count_AraServices { get; set; }

        private ArrayList mBaseServices = new ArrayList();
        private ArrayList mOneServices = new ArrayList();
        private ArrayList mAraServices = new ArrayList();

        private int p1;
        private int p2;
        private int p3;

        public void DemoServicesInt(int numberOfBaseServices, int numberOfOneServices, int numberOfAraServices)
        {
            count_AraServices = numberOfAraServices;
            count_OneServices = numberOfOneServices;
            count_BaseServices = numberOfBaseServices;
        }

        public DemoServices(string p1, string p2, string p3)
        {
            // TODO: Complete member initialization
            this.p1 = Convert.ToInt16(p1);
            this.p2 = Convert.ToInt16(p2);
            this.p3 = Convert.ToInt16(p3);

            DemoServicesInt(this.p1,this.p2,this.p3);
        }

        public void addBaseService(string baseService)
        {
            mBaseServices.Add(baseService);
        }

        public void addOneService(string oneService)
        {
            mOneServices.Add(oneService);
        }

        public void addAraService(string araService)
        {
            mAraServices.Add(araService);
        }

        public string getBaseService(int index)
        {
            return mBaseServices[index].ToString();
        }

        public string getOneService(int index)
        {
            return mOneServices[index].ToString();
        }

        public string getAraService(int index)
        {
            return mAraServices[index].ToString();
        }
    }

}
