namespace oed_testdata.Server.CloudEvents
{
    public class CloudEventType
    {
        public const string CaseStatusUpdated = "no.altinn.events.digitalt-dodsbo.v1.case-status-updated";
        public const string DeclarationSubmitted = "no.altinn.events.digitalt-dodsbo.v1.declaration-submitted";
        public const string WebhookValidation = "platform.events.validatesubscription";
        public const string HeirRolesUpdated = "no.altinn.events.digitalt-dodsbo.v1.heir-roles-updated";
    }

    public class OtherStuff
    {
        public const string ServiceResourceId = "urn:altinn:resource:dodsbo-domstoladmin-api";
        public const string AppProcessCompleted = "app.instance.process.completed";
    }
}
