﻿//  
// Copyright (c) Microsoft.  All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models
{
    using System;

    public class PsApiManagementProduct
    {
        public string ProductId { get; set; }

        // maps from Name
        public string Title { get; set; }

        public string Description { get; set; }

        // maps from Terms
        public string LegalTerms { get; set; }

        public bool? SubscriptionRequired { get; set; }

        public bool? ApprovalRequired { get; set; }

        public int? SubscriptionsLimit { get; set; }

        // maps from period contract
        public string SubscriptionPeriod { get; set; }

        // maps from period contract
        public string NotificationPeriod { get; set; }

        public PsApiManagementProductState State { get; set; }
    }
}