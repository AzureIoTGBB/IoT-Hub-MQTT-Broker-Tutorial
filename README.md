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
* Visual Studio 2017.
* Basic Linux skillset.
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
* Wait for the VM to deploy.
* Open Port 1883 inbound on the VM.
  * Go to the "Networking" setting for the VM.
  * Create a new inbound port rule to allow inbound traffic on Port 1883
  

### Install VerneMQ

Detailed instructions on installing VerneMQ are located [here](https://vernemq.com/docs/installation/).  Below is the subset of steps that I took.

* Retrieve your VM's IP Address.
* From the Azure portal, open the "Cloud Shell".  ">\_" in the top right corner.
* SSH into your VM from the cloud shell. 
* For simplicity, enter "sudo su".  This will put you in "super user" mode, and you won't have to put "sudo" in front of all of your commands.
* Create an install directory, and change directory to it
```
# mkdir install
# cd install
```
* Obtain the URL to the "Stable build" of the Ubuntu "Trusty" install [here](https://vernemq.com/downloads/)
* Retrieve the file
```
wget <URL>
```
* Install VerneMQ with the following command
```
dpkg -i <filename>
```
* Verify that it was installed correctly.  The status should be "Status: install ok installed"
```
dpkg -s vernemq | grep Status
```
* Edit the '/etc/vernemq/vernemq.conf' file
* Find the 'listener.tcp.default" configuration.  Set the default listener to allow incoming connections.
```
listener.tcp.default = "0.0.0.0:1883"
```
* Increase the open file limits.  Create a new file '/etc/security/limits.conf' in an editor.  Add the following to the file:
```
/etc/default/vernemq
```
* Start VerneMQ
```
vernemq start
```
