<?xml version='1.0' encoding='ASCII' ?>
<Velocity11 file='Protocol_Data' md5sum='5ab2b24f4e5d3cd70a0b04966f50faa1' version='2.0' >
	<File_Info AllowSimultaneousRun='1' AutoExportGanttChart='0' AutoLoadRacks='When the main protocol starts' AutoUnloadRacks='0' AutomaticallyLoadFormFile='1' Barcodes_Directory='' ClearInventory='0' DeleteHitpickFiles='1' Description='' Device_File='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\VWorks Device File\ATC.dev' Display_User_Task_Descriptions='1' DynamicAssignPlateStorageLoad='0' FinishScript='' Form_File='' HandlePlatesInInstance='1' ImportInventory='0' InventoryFile='' Notes='' PipettePlatesInInstanceOrder='0' Protocol_Alias='' StartScript='' Use_Global_JS_Context='0' />
	<Processes >
		<Main_Processes >
			<Process >
				<Minimized >0</Minimized>
				<Task Name='ATC::CLOSE' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='1' />
						<Parameter Category='Task Description' Name='Task description' Value='CLOSE (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::START' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='' Name='Protocol File Path' Value='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\ATC Protocols\OneCycleTest.xml' />
						<Parameter Category='Task Description' Name='Task number' Value='2' />
						<Parameter Category='Task Description' Name='Task description' Value='START (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::OPEN' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='3' />
						<Parameter Category='Task Description' Name='Task description' Value='OPEN (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Plate_Parameters >
					<Parameter Name='Plate name' Value='OneCycle' />
					<Parameter Name='Plate type' Value='96 Greiner 655101 PS Clr Rnd Well Flat Btm' />
					<Parameter Name='Simultaneous plates' Value='1' />
					<Parameter Name='Plates have lids' Value='0' />
					<Parameter Name='Plates enter the system sealed' Value='0' />
					<Parameter Name='Use single instance of plate' Value='1' />
					<Parameter Name='Automatically update labware' Value='0' />
					<Parameter Name='Enable timed release' Value='0' />
					<Parameter Name='Release time' Value='30' />
					<Parameter Name='Auto managed counterweight' Value='0' />
					<Parameter Name='Barcode filename' Value='No Selection' />
					<Parameter Name='Has header' Value='' />
					<Parameter Name='Barcode or header South' Value='No Selection' />
					<Parameter Name='Barcode or header West' Value='No Selection' />
					<Parameter Name='Barcode or header North' Value='No Selection' />
					<Parameter Name='Barcode or header East' Value='No Selection' />
				</Plate_Parameters>
				<Quarantine_After_Process >0</Quarantine_After_Process>
			</Process>
			<Process >
				<Minimized >0</Minimized>
				<Task Name='ATC::CLOSE' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='1' />
						<Parameter Category='Task Description' Name='Task description' Value='CLOSE (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::START' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='' Name='Protocol File Path' Value='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\ATC Protocols\FifteenCycleTest.xml' />
						<Parameter Category='Task Description' Name='Task number' Value='2' />
						<Parameter Category='Task Description' Name='Task description' Value='START (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::OPEN' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='3' />
						<Parameter Category='Task Description' Name='Task description' Value='OPEN (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Plate_Parameters >
					<Parameter Name='Plate name' Value='15 Cycles' />
					<Parameter Name='Plate type' Value='96 Greiner 655101 PS Clr Rnd Well Flat Btm' />
					<Parameter Name='Simultaneous plates' Value='1' />
					<Parameter Name='Plates have lids' Value='0' />
					<Parameter Name='Plates enter the system sealed' Value='0' />
					<Parameter Name='Use single instance of plate' Value='1' />
					<Parameter Name='Automatically update labware' Value='0' />
					<Parameter Name='Enable timed release' Value='0' />
					<Parameter Name='Release time' Value='30' />
					<Parameter Name='Auto managed counterweight' Value='0' />
					<Parameter Name='Barcode filename' Value='No Selection' />
					<Parameter Name='Has header' Value='' />
					<Parameter Name='Barcode or header South' Value='No Selection' />
					<Parameter Name='Barcode or header West' Value='No Selection' />
					<Parameter Name='Barcode or header North' Value='No Selection' />
					<Parameter Name='Barcode or header East' Value='No Selection' />
				</Plate_Parameters>
				<Quarantine_After_Process >0</Quarantine_After_Process>
			</Process>
			<Process >
				<Minimized >0</Minimized>
				<Task Name='ATC::CLOSE' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='1' />
						<Parameter Category='Task Description' Name='Task description' Value='CLOSE (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::START' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='4' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='' Name='Protocol File Path' Value='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\ATC Protocols\ThirtyCycleTest.xml' />
						<Parameter Category='Task Description' Name='Task number' Value='2' />
						<Parameter Category='Task Description' Name='Task description' Value='START (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::OPEN' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='3' />
						<Parameter Category='Task Description' Name='Task description' Value='OPEN (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Plate_Parameters >
					<Parameter Name='Plate name' Value='30 Cycles' />
					<Parameter Name='Plate type' Value='96 Greiner 655101 PS Clr Rnd Well Flat Btm' />
					<Parameter Name='Simultaneous plates' Value='1' />
					<Parameter Name='Plates have lids' Value='0' />
					<Parameter Name='Plates enter the system sealed' Value='0' />
					<Parameter Name='Use single instance of plate' Value='1' />
					<Parameter Name='Automatically update labware' Value='0' />
					<Parameter Name='Enable timed release' Value='0' />
					<Parameter Name='Release time' Value='30' />
					<Parameter Name='Auto managed counterweight' Value='0' />
					<Parameter Name='Barcode filename' Value='No Selection' />
					<Parameter Name='Has header' Value='' />
					<Parameter Name='Barcode or header South' Value='No Selection' />
					<Parameter Name='Barcode or header West' Value='No Selection' />
					<Parameter Name='Barcode or header North' Value='No Selection' />
					<Parameter Name='Barcode or header East' Value='No Selection' />
				</Plate_Parameters>
				<Quarantine_After_Process >0</Quarantine_After_Process>
			</Process>
			<Process >
				<Minimized >0</Minimized>
				<Task Name='BuiltIn::Loop' >
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings />
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='' Name='Number of times to loop' Value='5' />
						<Parameter Category='' Name='Change tips every N times, N = ' Value='1' />
					</Parameters>
					<Variables />
				</Task>
				<Task Name='ATC::CLOSE' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='2' />
						<Parameter Category='Task Description' Name='Task description' Value='CLOSE (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::START' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='' Name='Protocol File Path' Value='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\ATC Protocols\ThirtyCycleTest.xml' />
						<Parameter Category='Task Description' Name='Task number' Value='3' />
						<Parameter Category='Task Description' Name='Task description' Value='START (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::OPEN' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='4' />
						<Parameter Category='Task Description' Name='Task description' Value='OPEN (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='BuiltIn::Loop End' >
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings />
					<TaskScript Name='TaskScript' Value='' />
				</Task>
				<Plate_Parameters >
					<Parameter Name='Plate name' Value='Loop' />
					<Parameter Name='Plate type' Value='96 Greiner 655101 PS Clr Rnd Well Flat Btm' />
					<Parameter Name='Simultaneous plates' Value='1' />
					<Parameter Name='Plates have lids' Value='0' />
					<Parameter Name='Plates enter the system sealed' Value='0' />
					<Parameter Name='Use single instance of plate' Value='1' />
					<Parameter Name='Automatically update labware' Value='0' />
					<Parameter Name='Enable timed release' Value='0' />
					<Parameter Name='Release time' Value='30' />
					<Parameter Name='Auto managed counterweight' Value='0' />
					<Parameter Name='Barcode filename' Value='No Selection' />
					<Parameter Name='Has header' Value='' />
					<Parameter Name='Barcode or header South' Value='No Selection' />
					<Parameter Name='Barcode or header West' Value='No Selection' />
					<Parameter Name='Barcode or header North' Value='No Selection' />
					<Parameter Name='Barcode or header East' Value='No Selection' />
				</Plate_Parameters>
				<Quarantine_After_Process >0</Quarantine_After_Process>
			</Process>
		</Main_Processes>
	</Processes>
</Velocity11>