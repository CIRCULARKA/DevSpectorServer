SELECT Devices.ID, InventoryNumber, DeviceTypes.Name as [Type], NetworkName FROM Devices
LEFT JOIN DeviceTypes ON DeviceTypeID = DeviceTypes.ID;
