import pytest,time,sys,os
sys.path.append(os.path.dirname(os.path.realpath(__file__)) + "/../../")
import rticonnextdds_connector as rti

class TestDataFlow:

  def test_inp_exists(self,inp):
    assert inp!=None and isinstance(inp,rti.Input) 

  def test_out_exists(self,out):
    assert out!=None and isinstance(out,rti.Output) 

  def test_dataflow_with_read(self,inp,out):
    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
    out.instance.setDictionary(msg)
    out.write()
    received_count=0
    received_msg={}
    for i in range(1,20):
	time.sleep(.5)
        inp.read()
        numOfSamples  = inp.samples.getLength()
	if numOfSamples > 0:
          received_count+= numOfSamples
          for j in range(1, numOfSamples+1):
              if inp.infos.isValid(j):
                received_msg = inp.samples.getDictionary(j)
        if received_count==1:
          break
    assert received_count==1 and received_msg==msg

  @pytest.mark.skip
  # TODO: Figure out how to take data on the basis of read/seen status
  def test_dataflow_with_take(self,inp,out):
    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
    out.instance.setDictionary(msg)
    out.write()
    received_count=0
    received_msg={}
    for i in range(1,20):
	time.sleep(.5)
        inp.take()
        numOfSamples  = inp.samples.getLength()
	if numOfSamples > 0:
          received_count+= numOfSamples
          for j in range(1, numOfSamples+1):
            if inp.infos.isValid(j):
              x = inp.samples.getNumber(j,"x")
              y = inp.samples.getNumber(j,"y")
              color = inp.samples.getString(j,"color")
              shapesize  = inp.samples.getNumber(j,"shapesize")
              received_msg={"x":x,"y":y,"color":color,"shapesize":shapesize}
        if received_count==1:
          break
    assert received_count==1 and received_msg==msg

  # TODO: accessing non-existent field should throw an exception
  def test_getNumber_for_nonexistent_field(self,inp,out,capfd):
    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
    out.instance.setDictionary(msg)
    out.write()
    received_count=0
    for i in range(1,20):
	time.sleep(.5)
        inp.take()
        numOfSamples  = inp.samples.getLength()
	if numOfSamples > 0:
          received_count+= numOfSamples
          for j in range(1, numOfSamples+1):
            if inp.infos.isValid(j):
              m = inp.samples.getNumber(j,"m")
        if received_count==1:
          break
    out,err=capfd.readouterr()
    assert "DynamicData_get:!get kind failed" in out

  def test_getString_for_nonexistent_field(self,inp,out,capfd):
    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
    out.instance.setDictionary(msg)
    out.write()
    received_count=0
    for i in range(1,20):
	time.sleep(.5)
        inp.take()
        numOfSamples  = inp.samples.getLength()
	if numOfSamples > 0:
          received_count+= numOfSamples
          for j in range(1, numOfSamples+1):
            if inp.infos.isValid(j):
              m = inp.samples.getString(j,"m")
        if received_count==1:
          break
    out,err=capfd.readouterr()
    assert "DynamicData_get:!get kind failed" in out

  def test_getBoolean_for_nonexistent_field(self,inp,out,capfd):
    msg={"x":1,"y":1,"color":"BLUE","shapesize":5}
    out.instance.setDictionary(msg)
    out.write()
    received_count=0
    for i in range(1,20):
	time.sleep(.5)
        inp.take()
        numOfSamples  = inp.samples.getLength()
	if numOfSamples > 0:
          received_count+= numOfSamples
          for j in range(1, numOfSamples+1):
            if inp.infos.isValid(j):
              m = inp.samples.getBoolean(j,"m")
        if received_count==1:
          break
    out,err=capfd.readouterr()
    assert "DynamicData_get:!get kind failed" in out
  
  # TODO: tests for invalid type access for methods getNumber,getString & getBoolean
  """
  getString on numeric field gives a string representation of the number. 
  getString on boolean field gives string representation of True/False
 
  getBoolean on string or numeric field gives 0/1
  
  getNumber on a boolean field gives 0/1 
  getNumber on string field gives a random number
  """
