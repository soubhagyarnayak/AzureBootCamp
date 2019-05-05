```bash
az login
az storage account create -n azurebootcampapr1 -g Bootcamp
az storage container create -n testnayak --account-name azurebootcampapr1
az storage blob upload -f MSFT.png -c testnayak -n profilepic --account-name azurebootcampapr1
az storage account delete -n azurebootcampapr1 -g Bootcamp
```