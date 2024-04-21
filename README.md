



**Introduction**<br>
A command line tool to assist in preparing training image data for pytorch-CycleGAN-and-pix2pix and PyTorch-UNet. This tool allows you to focus on your project and tasks.

**Installation**<br>
You can download tools according to your needs there are two tools one for pytorch-CycleGAN-and-pix2pix and the other one for PyTorch-UNet. They are separated by OS and CPU architecture. If your system does not exist you can submit an issue or use the portable version which needs to install .Net runtime on your OS. you can find more information about installing .Net runtime on your OS in the link.

**Usage**
<br>The pytorch-CycleGAN-and-pix2pix helper flags list. <br>
1. -i|--input = the path of input folder. The folder contains all data.
2. -o|--output = The output folder path. system create all folder inside automaticlly. by default is `./result`.
3. -v|--vali = The chance of an image go to validator set. Up to 100.
4. -t|--test = The chance of an image go to test set. Up tp 100.
5. -a|--allow= the list of Allowed Extensions for images.
<hr>
<br>
The PyTorch-UNet helper flags list. <br>
1. -i|--input = the path of input folder. The folder contains all data.<br>
2. -o|--output = The output folder path. system create all folder inside automaticlly. by default is `./result`.<br>
3. -a|--allow= the list of Allowed Extensions for images.


**Examples**<br>
The command for the Pytorch-UNet tool. <br>
```
'.\Pytorch-UNet prepareimages.exe' --input=F:\Work\Universty\sensor-stimle\windows\pytorch-CycleGAN-and-pix2pix\datasets\my-project\train
```
The output <br>
```
Output: F:\Repos\sharplab prepearimages\Pytorch-UNet prepareimages\bin\Debug\net8.0\result\imgs
Output: F:\Repos\sharplab prepearimages\Pytorch-UNet prepareimages\bin\Debug\net8.0\result\masks
Done!
Have nice day
```


