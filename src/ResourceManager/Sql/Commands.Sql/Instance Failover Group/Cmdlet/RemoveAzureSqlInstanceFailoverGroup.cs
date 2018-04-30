// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.Sql.InstanceFailoverGroup.Model;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation;
using System;

namespace Microsoft.Azure.Commands.Sql.InstanceFailoverGroup.Cmdlet
{
    [Cmdlet(VerbsCommon.Remove, "AzureRmSqlDatabaseInstanceFailoverGroup", DefaultParameterSetName = RemoveIFGDefaultSet,
        SupportsShouldProcess = true), OutputType(typeof(AzureSqlInstanceFailoverGroupModel))]
    public class RemoveAzureSqlInstanceFailoverGroup : AzureSqlInstanceFailoverGroupCmdletBase
    {
        /// <summary>
        /// Parameter set name for the default remove.
        /// </summary>
        private const string RemoveIFGDefaultSet = "RemoveIFGDefault";

        /// <summary>
        /// Parameter set name for remove with an Input Object.
        /// </summary>
        protected const string RemoveIFGByInputObjectParameterSet =
            "Remove a Instance Failover Group from AzureSqlInstanceFailoverGroupModel instance definition";
        
        /// <summary>
        /// Parameter set name for remove with a resource ID.
        /// </summary>
        private const string RemoveIFGByResourceIdSet = "Remove a Instance Failover Group from Resource Id";
        
        /// <summary>
        /// Gets or sets the name of the InstanceFailoverGroup to remove.
        /// </summary>
        [Parameter(ParameterSetName = RemoveIFGDefaultSet, 
            Mandatory = true,
            Position = 2,
            HelpMessage = "The name of the Instance Failover Group to remove.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
	    /// Instance Failover Group object to remove
	    /// </summary>
	    [Parameter(ParameterSetName = RemoveIFGByInputObjectParameterSet,
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "The Instance Failover Group object to remove")]
        [ValidateNotNullOrEmpty]
        public AzureSqlInstanceFailoverGroupModel InputObject { get; set; }

        /// <summary>
        /// Gets or sets the resource ID of the Instance Failover Group to remove.
        /// </summary>
        [Parameter(ParameterSetName = RemoveIFGByResourceIdSet,
            Mandatory = true,
            Position = 0,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The Resource ID of the Instance Failover Group to remove.")]
        [ValidateNotNullOrEmpty]
        public string ResourceId { get; set; }

        /// <summary>
        /// Defines whether it is ok to skip the requesting of rule removal confirmation
        /// </summary>
        [Parameter(HelpMessage = "Skip confirmation message for performing the action.")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Get the entities from the service
        /// </summary>
        /// <returns>The list of entities</returns>
        protected override IEnumerable<AzureSqlInstanceFailoverGroupModel> GetEntity()
        {
            return new List<AzureSqlInstanceFailoverGroupModel>() {
                ModelAdapter.GetInstanceFailoverGroup(this.ResourceGroupName, this.Location, this.Name),
            };
        }

        /// <summary>
        /// No user input to apply to model
        /// </summary>
        /// <param name="model">Model retrieved from service</param>
        /// <returns>The model that was passed in</returns>
        protected override IEnumerable<AzureSqlInstanceFailoverGroupModel> ApplyUserInputToModel(IEnumerable<AzureSqlInstanceFailoverGroupModel> model)
        {
            return model;
        }

        /// <summary>
        /// Persist deletion
        /// </summary>
        /// <param name="entity">The output of apply user input to model</param>
        /// <returns>The input entity</returns>
        protected override IEnumerable<AzureSqlInstanceFailoverGroupModel> PersistChanges(IEnumerable<AzureSqlInstanceFailoverGroupModel> entity)
        {
            ModelAdapter.RemoveInstanceFailoverGroup(this.ResourceGroupName, this.Location, this.Name);
            return entity;
        }

        /// <summary>
        /// Entry point for the cmdlet
        /// </summary>
        public override void ExecuteCmdlet()
        {
            if (InputObject != null)
            {
                Location = InputObject.Location;
                Name = InputObject.Name;
                ResourceGroupName = InputObject.ResourceGroupName;
            }
            else if (!string.IsNullOrWhiteSpace(ResourceId))
            {
               ResourceIdentifier identifier = new ResourceIdentifier(ResourceId);
                Location = identifier.ResourceName;
                identifier = new ResourceIdentifier(identifier.ParentResource);
                Name = identifier.ResourceName;
                ResourceGroupName = identifier.ResourceName;
            }
            if (!Force.IsPresent && !ShouldProcess(
               string.Format(CultureInfo.InvariantCulture, Microsoft.Azure.Commands.Sql.Properties.Resources.RemoveAzureSqlDatabaseInstanceFailoverGroupDescription, this.Name, this.Location),
               string.Format(CultureInfo.InvariantCulture, Microsoft.Azure.Commands.Sql.Properties.Resources.RemoveAzureSqlDatabaseInstanceFailoverGroupWarning, this.Name, this.Location),
               Microsoft.Azure.Commands.Sql.Properties.Resources.ShouldProcessCaption))
            {
                return;
            }

            base.ExecuteCmdlet();
        }
    }
}
