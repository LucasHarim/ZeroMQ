import zmq
import time
import json

topics_ports = { 'propulsao/forcas': '5555',
                'propulsao/momentos': '5556',
                'propulsao/potencias': '5557'}
                

class ZMQPub:
    def __init__(self, port: str, topic: str):
        self.port = port
        self.topic = topic

        context = zmq.Context()
        self.socket = context.socket(zmq.PUB)
        print('tcp://*:%s' % self.port)
        self.socket.bind('tcp://127.0.0.1:%s' % self.port)
        
    def publish(self, message_data: str, rate: float):
        
        # topic_message = self.topic + " " + message_data
        self.socket.send_json(message_data)
        tsleep = 1/rate
        time.sleep(tsleep)



class ZMQSub:
    def __init__(self, port: str,topic: str):
        self.port = port
        self.topic = topic
        
        context = zmq.Context()
        self.socket = context.socket(zmq.SUB)
        
        self.socket.connect ('tcp://127.0.0.1:%s' % self.port)
        self.socket.setsockopt_string(zmq.SUBSCRIBE, self.topic)

    def receive(self):
        
        data_received = self.socket.recv()
        # data_received = json.loads(data_received)
        
        return data_received
        