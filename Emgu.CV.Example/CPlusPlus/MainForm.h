#pragma once


namespace CPlusPlus {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Summary for MainForm
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class MainForm : public System::Windows::Forms::Form
	{
	public:
		MainForm(array<Emgu::CV::IImage^>^ image)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			imageBox1->Image = image[0];
			imageBox2->Image = image[1];
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~MainForm()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::SplitContainer^  splitContainer1;
	protected: 


	private: Emgu::CV::UI::ImageBox^  imageBox1;
	private: System::Windows::Forms::Panel^  panel1;
	private: System::Windows::Forms::Label^  label1;
	private: Emgu::CV::UI::ImageBox^  imageBox2;
	private: System::Windows::Forms::Panel^  panel2;
	private: System::Windows::Forms::Label^  label2;

	protected: 

	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->splitContainer1 = (gcnew System::Windows::Forms::SplitContainer());
			this->panel1 = (gcnew System::Windows::Forms::Panel());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->imageBox1 = (gcnew Emgu::CV::UI::ImageBox());
			this->panel2 = (gcnew System::Windows::Forms::Panel());
			this->imageBox2 = (gcnew Emgu::CV::UI::ImageBox());
			this->label2 = (gcnew System::Windows::Forms::Label());
			this->splitContainer1->Panel1->SuspendLayout();
			this->splitContainer1->Panel2->SuspendLayout();
			this->splitContainer1->SuspendLayout();
			this->panel1->SuspendLayout();
			this->panel2->SuspendLayout();
			this->SuspendLayout();
			// 
			// splitContainer1
			// 
			this->splitContainer1->Dock = System::Windows::Forms::DockStyle::Fill;
			this->splitContainer1->Location = System::Drawing::Point(0, 0);
			this->splitContainer1->Name = L"splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this->splitContainer1->Panel1->Controls->Add(this->imageBox1);
			this->splitContainer1->Panel1->Controls->Add(this->panel1);
			// 
			// splitContainer1.Panel2
			// 
			this->splitContainer1->Panel2->Controls->Add(this->imageBox2);
			this->splitContainer1->Panel2->Controls->Add(this->panel2);
			this->splitContainer1->Size = System::Drawing::Size(829, 304);
			this->splitContainer1->SplitterDistance = 429;
			this->splitContainer1->TabIndex = 1;
			// 
			// panel1
			// 
			this->panel1->Controls->Add(this->label1);
			this->panel1->Dock = System::Windows::Forms::DockStyle::Top;
			this->panel1->Location = System::Drawing::Point(0, 0);
			this->panel1->Name = L"panel1";
			this->panel1->Size = System::Drawing::Size(429, 43);
			this->panel1->TabIndex = 3;
			// 
			// label1
			// 
			this->label1->AutoSize = true;
			this->label1->Location = System::Drawing::Point(12, 18);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(74, 13);
			this->label1->TabIndex = 3;
			this->label1->Text = L"Original Image";
			// 
			// imageBox1
			// 
			this->imageBox1->DisplayedImage = nullptr;
			this->imageBox1->Dock = System::Windows::Forms::DockStyle::Fill;
			this->imageBox1->Image = nullptr;
			this->imageBox1->Location = System::Drawing::Point(0, 43);
			this->imageBox1->Name = L"imageBox1";
			this->imageBox1->Size = System::Drawing::Size(429, 261);
			this->imageBox1->TabIndex = 4;
			// 
			// panel2
			// 
			this->panel2->Controls->Add(this->label2);
			this->panel2->Dock = System::Windows::Forms::DockStyle::Top;
			this->panel2->Location = System::Drawing::Point(0, 0);
			this->panel2->Name = L"panel2";
			this->panel2->Size = System::Drawing::Size(396, 41);
			this->panel2->TabIndex = 3;
			// 
			// imageBox2
			// 
			this->imageBox2->DisplayedImage = nullptr;
			this->imageBox2->Dock = System::Windows::Forms::DockStyle::Fill;
			this->imageBox2->Image = nullptr;
			this->imageBox2->Location = System::Drawing::Point(0, 41);
			this->imageBox2->Name = L"imageBox2";
			this->imageBox2->Size = System::Drawing::Size(396, 263);
			this->imageBox2->TabIndex = 4;
			// 
			// label2
			// 
			this->label2->AutoSize = true;
			this->label2->Location = System::Drawing::Point(12, 18);
			this->label2->Name = L"label2";
			this->label2->Size = System::Drawing::Size(88, 13);
			this->label2->TabIndex = 4;
			this->label2->Text = L"Image with Noise";
			// 
			// MainForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(829, 304);
			this->Controls->Add(this->splitContainer1);
			this->Name = L"MainForm";
			this->Text = L"MainForm";
			this->splitContainer1->Panel1->ResumeLayout(false);
			this->splitContainer1->Panel2->ResumeLayout(false);
			this->splitContainer1->ResumeLayout(false);
			this->panel1->ResumeLayout(false);
			this->panel1->PerformLayout();
			this->panel2->ResumeLayout(false);
			this->panel2->PerformLayout();
			this->ResumeLayout(false);

		}
#pragma endregion
	};
}

