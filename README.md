# MQTT Broker to Azure IoT Hub Tutorial

**Please Note this repo is currently "work in progress". ** 

**CAVEAT: This sample is to demonstrate azure IoT client concepts only and is not a guide design principles or style.  Proper exception management is omitted for brevity.  Please practice sound engineering practices when writing production code.**

This tutorial will walk you through the following steps:
1) Create and configure an MQTT Broker (VerneMQ) in an Azure Ubuntu Linux VM.
1) Send messages to the the MQTT Broker using a C# app.
1) Receive messages from an MQTT Broker and send them to an Azure IoT Hub.

## Prerequisites

You will need

* an Azure subscription.  If you do not already have one, you can create an Azure account and subscription with free credits [here](https://azure.microsoft.com/en-ca/free)
* an IoT Hub.  If you do not already have one, create one via the instructions [here](https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-create-using-cli#create-an-iot-hub)
* Visual Studio 2017.
* some basic Linux skills (directory management, file editing, etc.)
* an intermediate understanding of how to create resources in Azure.  Field by field instructions are not provided.

##  Create and Configure an MQTT Broker in an Azure VM

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
* Find the 'allow_anonymous' configuration.  Set it to "on".  
  * NOTE: This will allow anonymous connections to your MQTT Broker and is not recommended for production installations.
* Find the 'listener.tcp.default' configuration.  Set the default listener to allow incoming connections.
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
### Useful VerneMQ Commands
Below are some useful Linux/VerneMQ commands.  They are located in the cmds directory in this repo.  I place them in a 'cmds' directory on my Linux VM, and make them executable.  eg:
```
# mkdir cmds
# cd cmds
```
Copy files to the cmds directory.  Then set permissions:
```
# chmod 700 cmds
```
Then you can execute them by placing a './' in front of them.  eg:
```
./vstart
```
Here are the commands
```
Command  Description                           Command
=======  =================================     =================================
vmstart  Start                                 vernemq start
vmstop   Stop                                  vernemq stop
vconf    Edit the Configuration File           vi /etc/vernemq/vernemq.conf
vconsole Open the Console                      vernemq console
vdebug   View Debug information                vernemq config generate -l debug
vlog     View Log Data                         tail -f /var/log/vernemq/console.log
vnetstat View services listening on port 1883  netstat -p tcp -ano | grep "1883"
vping    Ping VerneMQ                          vernemq ping
``` 

## Configure the Azure IoT Hub

Create the IoT Hub.  
* Open the [Azure Portal](https://portal.azure.com)
* Click "+ Create a resource".  (top left)
* In the "Search the Marketplace" box, enter "Iot Hub"
* Click on "IoT Hub"
* Click "Create"
* Enter the name of your hub
* Accept defaults. Any SKU including the "Free" or "Basic" SKUs will work.
* Click on "Review and Create"
* Click "Create"
  
Wait for the IoT Hub to be provisioned, then go to the new IoT Hub in the Portal.

Create a device to send the MQTT messages to:

* Click on the "IoT Devices" blade under "Explorers"
* Click "+ Add"
* Type in a name for your device
* Click "Save"
* Click on the device
* Copy the connection string.  We will need it later.

## Configuring the C# App

First clone the repo:
```
git clone https://github.com/AzureIoTGBB/IoT-Hub-MQTT-Broker-Tutorial 
```
Open AzureMQTTSendReceive.sln in Visual Studio 2017, then open the App.config file.  

* Set the "MQTTDevice" connection string to the connection string for your device
* Set the "MQTTBrokerAddress" to either the domain name or the ip address of your MQTT server
* Leave "MQTTUsername" blank ("")
* Leave "MQTTPassword" blank ("")

Compile the solution.  

Make two copies of the binaries (debug directory), one for sending and one for receiving.  In "send" mode the code will send the CPU and Memory values from your PC to the MQTT Broker.  eg. {"CPU":32.88946,"Memory":6513.0}  In "receive" mode, the code will receive messages from the MQTT Broker and send them to the IoT Hub.
```
<directory1>\AzureMQTTSendReceive.exe send
<directory2>\AzureMQTTSendReceive.exe receive
```
To view the messages from the IoT Hub, use the az command line tool on your PC.
* Open the [Azure Portal](http://portal.azure.com) in your browser
* click on the "Cloud Shell" button on the menu bar across the top
* install the azure iot cli extension with the following command
```bash
az extension add --name azure-cli-iot-ext
```
* once installed, you can monitor events coming into your IoT Hub with the following command:
```
az iot hub monitor-events -n <IoT Hub Name>
```

That's it!
