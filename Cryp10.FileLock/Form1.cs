﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cryp10.FileLock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void selectFilesToEncryptButton_Click(object sender, EventArgs e)
        {
            var result = openFilesToEncryptDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var filePaths = openFilesToEncryptDialog.FileNames;
                var fileInfo = new List<FileInfo>();

                foreach (var file in filePaths)
                {
                    fileInfo.Add(new FileInfo(file));
                }

                selectedFilesToEncryptListBox.Items.Add(fileInfo[0].FullName);
            }
        }

        private void selectEncryptionFilesButton_Click(object sender, EventArgs e)
        {
            var result = openFilesForEncryptionDialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                var filePaths = openFilesForEncryptionDialog.FileNames;
                var fileInfo = new List<FileInfo>();

                foreach(var file in filePaths)
                {
                    fileInfo.Add(new FileInfo(file));
                }

                selectedFilesForEncryptionListBox.Items.Add(fileInfo[0].FullName);
            }
        }

        private void encryptFilesButton_Click(object sender, EventArgs e)
        {
            var text = "hello, i am cody";
            var key = Guid.NewGuid().ToByteArray();
            var iv = Guid.NewGuid().ToByteArray();

            byte[] encrypted = EncryptStringToBytesAes(text, key, iv);
            var ivAsString = System.Text.Encoding.Default.GetString(iv);
            var encAsString = System.Text.Encoding.Default.GetString(encrypted);

            var encryptedString = $"{ivAsString}_{encAsString}";

            richTextBox1.Text = text;
            richTextBox2.Text = encryptedString;
        }

        static byte[] EncryptStringToBytesAes(string text, byte[] key, byte[] iv)
        {
            byte[] encryptedText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create an encryptor for the stream encryption operations
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            //Write all data to the stream.
                            sw.Write(text);
                        }
                        encryptedText = ms.ToArray();
                    }
                }
            }

            return encryptedText;
        }
    }
}