# MQTT Broker to Azure IoT Hub Tutorial

NOTE: This repo is currently "work in progress".  

This tutorial will walk you through the following steps:
1) Create and configure an MQTT Broker (VerneMQ) in an Azure Ubuntu Linux VM.
1) Send messages to the the MQTT Broker using a C# app.
1) Receive messages from an MQTT Broker and send them to an Azure IoT Hub.

## Prerequisites

You will need

* an Azure subscription.  If you do not already have one, you can create an Azure account and subscription with free credits [here](https://azure.microsoft.com/en-ca/free)
* an IoT Hub.  If you do not already have one, create one via the instructions [here](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-using-cli#create-an-iot-hub)
* an intermediate understanding of how to create resources in Azure.  Field by field instructions are not provided.

## Step 1 - Create and Configure an MQTT Broker in an Azure VM

### Create an Ubuntu 16.04 Linux VM

* Open the Azure Portal.  https://portal.azure.com/
* Click on "+ Create a Resource" in the top left
* In the "Search the Marketplace" box, enter "Ubuntu Server 16.04 LTS"
* Choose the canonical "Ubuntu Server 16.04 LTS" server.  Build the VM as you normally would, except in the following areas:
  * Basics. Enter fields as normal. Use password authentication.  Click "Next: Disks".
  * Disks. Accept defaults.  Standard HDD is fine if you want to save a little money.  Click "Next: Networking".
  * Networking.  Choose "Allow Selected Ports".  Select "SSH (22)".  Click "Next: Management".
  * Tags. Accept defaults. Click 
