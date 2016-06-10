using System;
using System.Linq;
using Microsoft.SharePoint;
using RecordLion.RecordsManager.SharePoint.Workflow;


namespace SampleWorkflowProvider
{
    public class CustomWorkflowProvider : WorkflowProvider
    {
        private const string PROVIDER_NAME = "SampleWorkflowProvider.CustomWorkflowProvider";

        public override string DisplayName
        {
            get { return "Custom Workflow Provider"; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(CustomWorkflowProvider.PROVIDER_NAME, config);
        }

        public override bool StartWorkflow(WorkflowContext context)
        {
            //Do anything here...start a third-party workflow, call a web service, modify SharePoint content, etc.
             
            //This just does something for demonstration purposes...
            context.ListItem[SPBuiltInFieldId.Title] = "Provider Updated";
            context.ListItem.Update();

            return true;
        }
    }
}
