﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.DTOs.Payment
{
    public record PaymentResponse(
         int error,
         string message,
         object? data
    );
}
