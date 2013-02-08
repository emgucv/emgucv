// CPlusPlus.cpp : main project file.

#include "stdafx.h"
#include "MainForm.h"
#include "ImageProcessor.h"

using namespace CPlusPlus;

[STAThreadAttribute]
int main(array<System::String ^> ^args)
{
   // Enabling Windows XP visual effects before any controls are created
   Application::EnableVisualStyles();
   Application::SetCompatibleTextRenderingDefault(false); 

   // Create the main window and run it
   Application::Run(gcnew MainForm(ImageProcessor::ProcessImage() ));
   return 0;
}
