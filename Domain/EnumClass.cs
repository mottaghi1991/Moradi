using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  
    public static class EventIdList
    {
        public static int InsertId = 1000;
        public static int DeleteId = 1001;
        public static int UpdateId = 1002;
        public static int BulkInsertId = 1003;
        public static int BulkeDelete = 1004;
        public static int Login = 1005;
        public static int APi = 1006;
        public static int Read = 1007;
        public static int Error = 1008;
        public static int NotFound = 1009;
        public static int Info = 1010;
    }
    public enum FieldType
    {
        text= 1,
        textarea=2,
        textarea2=3,
        number=4,
        file=5,
        label=6,
        select=7
    }
    public enum UserDietstatus
    {
        FillForm = 1,
        Pay = 2,
        send = 3
    }
    public enum EntityType
    {
        Home=1,
        Diet=2,
        Product=3
    }
    public enum Gender
    {
        man ,
        woman 
    }
    public enum Stone
    {
        dorosht,
        motevaset,
        riz
    }
    public enum FormType
    {
        firstform=1,
        secondform=0
    }
    public enum OrderStatus
    {
        Pending,      // در انتظار پرداخت
        Paid,         // پرداخت شده
        Shipped,      // ارسال شده
        Delivered,    // تحویل داده شده
        Cancelled     // لغو شده
    }
}
