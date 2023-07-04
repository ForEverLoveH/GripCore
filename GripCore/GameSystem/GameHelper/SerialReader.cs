using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Timers;
using GripCore.GameSystem.GameSystemModel;
using Newtonsoft.Json;

namespace GripCore.GameSystem.GameHelper
{

   public delegate void ReciveDataCallBack(byte[] data);

   public delegate void SendDataCallBack(MachineMsgCode btArySendData);

   public delegate void AnalyDataCallBack(SerialMsg btAryAnalyData);

   public class SerialReader
   {
      public SerialPort iSerialPort;
      private int m_nType = -1;
      public ReciveDataCallBack ReceiveCallback;
      public SendDataCallBack SendCallback;
      public AnalyDataCallBack AnalyCallback;
      private System.Timers.Timer waitTimer;
      public string qType = "";
      public string mac = string.Empty;
      public string itemtype = string.Empty;
      public List<byte> _buffer = new List<byte>();
      public int number;

      public SerialReader(int number)
      {
         iSerialPort = new SerialPort();
         iSerialPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedComData);
         this.number = number;
      }

      public int OpenCom(string strPort, int nBaudrate, out string strException)
      {
         strException = string.Empty;
         if (iSerialPort.IsOpen)
         {
            iSerialPort.Close();
         }

         try
         {
            iSerialPort.PortName = strPort;
            iSerialPort.BaudRate = nBaudrate;
            iSerialPort.StopBits = StopBits.One;
            iSerialPort.Parity = Parity.None;
            iSerialPort.ReadTimeout = 30;
            iSerialPort.WriteTimeout = 1000;
            iSerialPort.ReadBufferSize = 4096 * 10;
            //iSerialPort.ReceivedBytesThreshold = 32;
            iSerialPort.Open();
            if (waitTimer != null)
            {
               if (waitTimer.Enabled)
               {
                  waitTimer.Stop();
               }

               waitTimer.Dispose();
               waitTimer = null;
            }

            //建立定时器处理数据
            waitTimer = new System.Timers.Timer(30); //实例化Timer类，设置间隔时间为10000毫秒；
            waitTimer.Elapsed += new System.Timers.ElapsedEventHandler(AnalyReceivedData); //到达时间的时候执行事件；
            waitTimer.AutoReset = true; //设置是执行一次（false）还是一直执行(true)；
            waitTimer.Enabled = true; //是否执行System.Timers.Timer.Elapsed事件；
            waitTimer.Start(); //启动定时器
         }
         catch (System.Exception ex)
         {
            Console.WriteLine(ex.Message);
            strException = ex.Message;
            return -1;
         }

         m_nType = 0;
         return 0;
      }

      public void CloseCom()
      {
         if (iSerialPort.IsOpen)
         {
            iSerialPort.Close();
         }

         if (waitTimer != null)
         {
            if (waitTimer.Enabled)
            {
               waitTimer.Stop();
            }

            waitTimer.Dispose();
            waitTimer = null;
         }

         m_nType = -1;
      }

      /// <summary>
      /// 串口是否开启
      /// </summary>
      /// <returns></returns>
      public bool IsComOpen()
      {
         try
         {
            return iSerialPort.IsOpen;
         }
         catch (Exception)
         {

            return false;
         }

      }

      //缓存
      byte[] s232Buffer = new byte[2048];
      int s232Buffersp = 0;

      private void ReceivedComData(object sender, SerialDataReceivedEventArgs e)
      {
         try
         {
            int nCount = iSerialPort.BytesToRead;
            if (nCount == 0)
            {
               return;
            }

            byte[] btAryBuffer = new byte[nCount];
            iSerialPort.Read(btAryBuffer, 0, nCount);
            //RunReceiveDataCallback(btAryBuffer);
            for (int i = 0; i < nCount; i++)
            {
               s232Buffer[s232Buffersp] = btAryBuffer[i];
               if (s232Buffersp < (s232Buffer.Length - 2))
                  s232Buffersp++;
            }

         }
         catch (System.Exception ex)
         {
            Console.WriteLine(ex.Message);
         }
      }

      /// <summary>
      /// 定时器处理数据
      /// </summary>
      /// <param name="source"></param>
      /// <param name="e"></param>
      private void AnalyReceivedData(object source, System.Timers.ElapsedEventArgs e)
      {
         if (waitTimer != null)
            waitTimer.Stop();
         try
         {
            if (s232Buffersp != 0)
            {
               byte[] btAryBuffer = new byte[s232Buffersp];
               Array.Copy(s232Buffer, 0, btAryBuffer, 0, s232Buffersp);
               Array.Clear(s232Buffer, 0, s232Buffersp);
               s232Buffersp = 0;
               _buffer.AddRange(btAryBuffer);
               int step = 1;
               while (_buffer.Count > 36 && step < _buffer.Count)
               {
                  if (_buffer[0] == 0x7B)
                  {
                     if (_buffer[step] != 0x7D)
                     {
                        step++;
                        if (step > _buffer.Count)
                        {
                           break;
                        }
                     }
                     else
                     {
                        byte[] receiveBytes = new byte[step + 1];
                        _buffer.CopyTo(0, receiveBytes, 0, step + 1);
                        RunReceiveDataCallback(receiveBytes);
                        _buffer.RemoveRange(0, step);
                     }
                  }
                  else
                  {
                     _buffer.RemoveAt(0);
                  }
               }
               //string code = CCommondMethod.ByteArrayToString(btAryBuffer, 0, btAryBuffer.Length);
               //Console.WriteLine($"------------------------receiveCount:{btAryBuffer.Length}   recv:{code}");
            }
         }
         catch (Exception ex)
         {
            LoggerHelper.Debug(ex);
         }
         finally
         {
            if (waitTimer != null)
               waitTimer.Start();
         }

      }

      private void RunReceiveDataCallback(byte[] btAryBuffer)
      {
         try
         {
            //{ 0x7B
            //} 0x7D

            // ReceiveCallback?.Invoke(btAryBuffer);
            int nCount = btAryBuffer.Length;
            string temp = ByteHelper.ParseToString(btAryBuffer);
            MachineMsgCode rValue = null;
            try
            {
               //{ 0x7B
               //} 0x7D
               rValue = JsonConvert.DeserializeObject<MachineMsgCode>(temp);
               Console.WriteLine($"---------------------------------------------Receive: ({temp})");
            }
            catch (Exception ex)
            {
               rValue = null;
               //LoggerHelper.Debug(ex);
            }

            if (rValue == null) return;

            switch (rValue.type)
            {
               case 1:
                  //设备登录
                  mac = rValue.mac;
                  itemtype = rValue.itemtype;
                  rValue.code = 1;
                  SendMessage(rValue);
                  break;
               case 2:
                  //设备开始测试返回命令
                  /* rValue.code = 1;
                   SendMessage(rValue);*/
                  break;
               case 3:
                  //获取成绩
                  rValue.code = 1;
                  //  SendMessage(rValue);
                  break;
               case 4:
                  //查询设备信息
                  break;
            }
            SerialMsg sm = new SerialMsg(rValue, number);
            AnalyCallback?.Invoke(sm);
         }
         catch (System.Exception ex)
         {
            Console.WriteLine(ex.Message);
         }
      }

      public int SendMessage(MachineMsgCode mmc)
      {
         //串口连接方式
         if (m_nType == 0)
         {
            if (!iSerialPort.IsOpen)
            {
               return -1;
            }

            mmc.mac = mac;
            mmc.itemtype = itemtype;
            string JsonStr = JsonConvert.SerializeObject(mmc);
            Console.WriteLine($"---------------------------------------------send: ({JsonStr})");
            byte[] btArySenderData = ByteHelper.GetBytes(JsonStr);
            iSerialPort.Write(btArySenderData, 0, btArySenderData.Length);
            SendCallback?.Invoke(mmc);

            return 0;
         }

         return -1;
      }

      public byte CheckValue(byte[] btAryData)
      {
         return CheckSum(btAryData, 0, btAryData.Length);
      }

      public static byte CheckSum(byte[] btAryBuffer, int nStartPos, int nLen)
      {
         byte btSum = 0x00;

         for (int nloop = nStartPos; nloop < nStartPos + nLen; nloop++)
         {
            btSum ^= btAryBuffer[nloop];
         }

         return btSum;
      }

   }
}