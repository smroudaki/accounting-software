using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoftware.Common
{
    public class Codes
    {
        public static Guid WaitingState = Guid.Parse("fe92b8f8-f206-4273-8ca8-f1ecf70a8197");
        public static Guid NotPassedState = Guid.Parse("3d905312-ae57-498c-a733-f5cbaf114940");
        public static Guid PassedState = Guid.Parse("b508bd08-5534-4d26-9ed4-c36575c8d90a");

        public static Guid CreditorType = Guid.Parse("d1605abf-8a46-4f2e-8e22-194298b9fd33");
        public static Guid DebtorType = Guid.Parse("749d242b-55f7-4361-bf1b-42c139411c49");
    }
}
