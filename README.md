# VWorks ATC Plugin

A VWorks plugin for the Thermo Fisher Scientific ATC (Automated Telescoping Conveyor).

This project is designed to be compiled as a 32-bit (x86) COM-visible library for integration with Agilent VWorks.

## Prerequisites

- Microsoft Visual Studio
- Agilent VWorks software

## Getting Started

1.  Clone the repository:
    ```bash
    git clone https://github.com/calico/vworks-atc-plugin.git
    cd vworks-atc-plugin
    ```

2.  Open the solution file (`.sln`) in Visual Studio.

## Building the Plugin

The plugin must be built as an **x86** assembly to be compatible with VWorks.

### Generating the Type Library (.tlb)
For VWorks to recognize the plugin via COM, a type library (`.tlb`) file must be generated and the assembly registered.

1.  **Run Visual Studio as an Administrator.** This is required for COM registration.
2.  Open the project's **Properties** (Right-click the project in Solution Explorer and select Properties).
3.  Go to the **Build** tab.
4.  Check the **Register for COM interop** checkbox.
5.  **Clean** and then **Rebuild** the solution. This is necessary to ensure the `.tlb` file is generated correctly.

This process will create the `.dll`, `.pdb`, and `.tlb` files in the output directory (e.g., `bin\x86\Debug`).

## Installation

1.  Copy the compiled plugin DLL (`vworks-atc-plugin.dll`) to the VWorks plugins directory. The default location is:
    ```
    C:\Program Files (x86)\Agilent Technologies\VWorks\Plugins
    ```
2.  For debugging, it is recommended to also copy the `.pdb` (debug symbols) and `.tlb` (type library) files to the same directory.

## Testing

You can use the `IWorksTest.exe` utility, found in the VWorks installation folder, to perform basic tests on the plugin. For the plugin to be listed and testable, the `.dll`, `.tlb`, and `.pdb` files must all be present in the `Plugins` directory.

## ATC Connection Notes

- On startup, the ATC device attempts to get an IP address from a DHCP server.
- If no DHCP server is available, it will self-assign an APIPA (Automatic Private IP Addressing) address, typically in the `169.254.x.x` range.
- For a reliable connection, ensure the host PC is on the same network subnet as the ATC. This can be achieved by using a DHCP server or by manually configuring the PC's network adapter settings.
