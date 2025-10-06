# oed-testdata-app
App to manage instance testdata for oed

## Configuration

### Required .NET Secrets
To run the application locally, you need to add the following configuration to your .NET user secrets if you want to use the "create estate" functionality. The values can be found in the key vault, and the client credentials are the same ones used in the test environment:

```json
{
  "MaskinportenSettings": {
    "ClientId": "",
    "EncodedJwk": "",
    "Environment": "test"
  }
}
```

To add these secrets, run the following command in your project directory:
```bash
dotnet user-secrets set "MaskinportenSettings:ClientId" "your-client-id"
dotnet user-secrets set "MaskinportenSettings:EncodedJwk" "your-encoded-jwk"
dotnet user-secrets set "MaskinportenSettings:Environment" "test"
```

## Estate Management

This application includes a tab panel interface for managing estate data. One of the tabs contains a form that allows manual entry of estate information:

### Estate Form Features
- Input fields for deceased and heir National Identification Numbers (NIN)
- Integration with the Tenor system for data validation and processing
- Tag management functionality for categorizing estates
- Generate button to automatically create deceased and heirs with their relations
- Automatic generation of two JSON files that together represent a complete estate record

### Important Notes
- The estate form functionality currently works only in local development environment
- Generated JSON files must be pushed to the main branch to persist estate data
- The two JSON files work together as a paired representation of each estate case

### Usage
1. Navigate to the estate management tab in the application
2. Either:
   - Fill in the required NIN fields for deceased and heir manually, OR
   - Use the generate button to automatically create deceased and heirs with their relations
3. Add relevant tags for categorization
4. Submit the form to generate the estate JSON files
5. Ensure the generated files are committed and pushed to main branch
