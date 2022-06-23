using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace Communication
{
    class NetMQPub
    {
        public string topic;
        public double rate;
        public string port;
        public PublisherSocket pubSocket;
        public string address;

        public NetMQPub(string topic, string port, double rate)
        {
            
            this.topic = topic;
            this.rate = rate;
            this.port = port;
            this.pubSocket = new PublisherSocket();
            this.pubSocket.Options.SendHighWatermark = 1000;
            this.address = String.Format("tcp://127.0.0.1:{0}", port);          
            this.pubSocket.Bind(this.address);
            
        }
        

        public void Publish(string message)
        {
            
            this.pubSocket.SendMoreFrame(this.topic).SendFrame(message);
 
            // Thread.Sleep(500);
        }
    }

    class NetMQSub
    {
        public string topic;
        public string port;
        public SubscriberSocket subSocket;
        public string address;
        public string messageTopicReceived;
        public string messageReceived;

        public NetMQSub(string topic, string port)
        {
            
            this.topic = topic;
            this.port = port;
            
            this.subSocket = new SubscriberSocket();
            this.subSocket.Options.ReceiveHighWatermark = 1000;
            this.address = String.Format("tcp://127.0.0.1:{0}", port);          
            this.subSocket.Connect(this.address);
            
            /*Subscribe("") sobrescreve todos os topicos publicados no endereco
            this.address. Adicionar um topico ( Subscribe("topic") ) inviabiliza
            o recebimento de mensagens enviadas por um Publisher em python*/
             
            this.subSocket.Subscribe("");
        }
        
        public void Receive()
        {   
            
            /*ReceiveFrameString() compromete a velocidade de recebimento dos 
            dados que realmente importam. Descomentar apenas se necessario 
            
            //! Existe um atraso considerável entre a mensagem enviada pelo publisher
            //! e o subscriber. 
            //TODO Procurar uma forma de resolver */
            
            // this.messageTopicReceived = this.subSocket.ReceiveFrameString();
            this.messageReceived = this.subSocket.ReceiveFrameString();
        }
    }
}    
