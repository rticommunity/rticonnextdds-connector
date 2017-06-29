
--
-- Copyright (c) 2005-2017 Real-Time Innovations, Inc. All rights reserved.
-- Permission to modify and use for internal purposes granted.
-- This software is provided "as is", without warranty, express or implied.
--

-- Globals (preserved across invocations)
if not count then
    print("Lua script init")
    count = 0
    -- we get a reference to the writer
    writer = CONTAINER.WRITER['MyPublisher::MyWriter']
else
    count = count + 1
end

print("*** iteration ", count,  "***")

-- here we get the value of the temperature set by the c program
local temp = CONTAINER.CONTEXT.temp
print("The temperature is ", temp)

if (temp > 80) then
    -- we set the value of the message...
    writer.instance["message"] = "It's too hot in here"
    print("Going to publis an alert ")
    -- ... and we write it!
    writer:write()
end
