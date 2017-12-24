# MiOrleans
Actor framework for interacting with Xiaomi Mi Smart Home devices using Microsoft's Orleans framework

## Process

Once developer mode has been enabled on the gateway, it regularly broadcasts heartbeat messages via UDP on port 9898. MiOrleans listens to these heartbeats and requests a device list from the broadcaster of the heartbest. When the device list is received, a reading is requesting from each of the devices in the device list. Finally, when the readings are received they are recorded.



## Messages

### Heartbeat

```{"cmd":"heartbeat","model":"gateway","sid":"7811dcb06972","short_id":"0","token":"mLD6CgJTTNiJe47k","data":"{\"ip\":\"192.168.2.12\"}"}```

### Get Id List

```{"cmd":"get_id_list"}```

### Get Id List - Ack

```{"cmd":"get_id_list_ack","sid":"7811dcb06972","token":"L0DI4IiFAvAgInyL","data":"[\"158d0001a200f5\",\"158d0001c1cdfb\"]"}```

### Read

```{"cmd":"read","sid":"7811dcb06972"}```

### Read - Ack from Gateway

```{"cmd":"read_ack","model":"gateway","sid":"7811dcb06972","short_id":0,"data":"{\"rgb\":0,\"illumination\":1292,\"proto_version\":\"1.0.9\"}"}```

### Read = Ack from Temperature/Humidity Sensor

```{"cmd":"read_ack","model":"sensor_ht","sid":"158d0001a200f5","short_id":60049,"data":"{\"voltage\":3005,\"temperature\":\"2120\",\"humidity\":\"5277\"}"}```

### Read - Ack from Door Sensor

```{"cmd":"read_ack","model":"magnet","sid":"158d0001c1cdfb","short_id":56258,"data":"{\"voltage\":3115,\"status\":\"open\"}"}```

### Report - Door Sensor

```{"cmd":"report","model":"magnet","sid":"158d0001c1cdfb","short_id":56258,"data":"{\"status\":\"open\"}"}```

### Read - Ack from Motion Sensor

```{"cmd":"read_ack","model":"motion","sid":"158d0001d525f8","short_id":50076,"data":"{\"voltage\":3055}"}```