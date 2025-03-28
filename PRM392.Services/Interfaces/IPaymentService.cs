using Net.payOS.Types;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> PayOsTransferHandler(WebhookType body);
        Task<ApplicationResponse> GetPaymentRequestInfo(int orderCode);
    }
}
