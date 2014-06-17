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
        public int count_WspServices { get; set; }
        public int count_WlsServices { get; set; }

        private ArrayList mBaseServices = new ArrayList();
        private ArrayList mOneServices = new ArrayList();
        private ArrayList mAraServices = new ArrayList();
        private ArrayList mWspServices = new ArrayList();
        private ArrayList mWlsServices = new ArrayList();

        private int p1;
        private int p2;
        private int p3;
        private int p4;
        private int p5;

        public void DemoServicesInt(int numberOfBaseServices, int numberOfOneServices, int numberOfAraServices, int numberOfWspServices, int numberOfWlsServices)
        {
            count_AraServices = numberOfAraServices;
            count_OneServices = numberOfOneServices;
            count_BaseServices = numberOfBaseServices;
            count_WspServices = numberOfWspServices;
            count_WlsServices = numberOfWlsServices;
        }

        public DemoServices(string p1, string p2, string p3, string p4, string p5)
        {
            // TODO: Complete member initialization
            this.p1 = Convert.ToInt16(p1);
            this.p2 = Convert.ToInt16(p2);
            this.p3 = Convert.ToInt16(p3);
            this.p4 = Convert.ToInt16(p4);
            this.p5 = Convert.ToInt16(p5);

            DemoServicesInt(this.p1,this.p2,this.p3,this.p4,this.p5);
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

        public void addWspService(string wspService)
        {
            mWspServices.Add(wspService);
        }

        public void addWlsService(string wlsService)
        {
            mWlsServices.Add(wlsService);
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

        public string getWspService(int index)
        {
            return mWspServices[index].ToString();
        }

        public string getWlsService(int index)
        {
            return mWlsServices[index].ToString();
        }
    }

}
