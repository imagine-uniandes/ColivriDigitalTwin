# Colivri Digital Twin

## About the Project

ColivriDigitalTwin is a Unity-based project that aims to create a digital replica of the Colivri laboratory at the University of Los Andes. The project facilitates experimentation and the development of new technologies, emphasizing virtual and augmented reality, simulators, and human-robot interfaces. This initiative is led by a team of students as part of their degree project within the Systems Engineering program at the university.

### Organization of Objects and Naming Conventions in Unity

In this project, the organization of objects should be structured hierarchically for easy navigation and maintenance. We utilize the UpperCamelCase convention, which applies to all objects, classes, and structures within Unity. Please adhere to this convention when contributing to the project.

#### Naming Scheme: UpperCamelCase

The UpperCamelCase convention implies that the first letter of each word is capitalized without spaces.

#### Example

![UpperCamelCaseExample](Images/UpperCamelCaseExample.jpg)

### Object Organization

Objects within Unity are organized in a clear hierarchical structure. In the example provided above, `FirstFloor` is a prefab, and `Floor`, `Column1`, `Walls` are child objects.

## Objectives

The primary objectives of the project are as follows:

1. Creating a precise virtual replica of the Colivri Laboratory in Unity.
2. Detailed representation of equipment, devices, and spaces within the laboratory.
3. Developing realistic and functional interactions with the television screens, allowing users to interact with them as a control center for the digital twin.
4. Exploring and optimizing the use of various virtual, augmented, and digital reality platforms for experiencing the Digital Twin.
5. Critically evaluating the effectiveness and utility of virtual television screens as essential components of the Digital Twin, particularly in terms of learning and simulation for students, professionals, and users of the Colivri Laboratory in the realm of virtual and augmented reality.

## Functionality

The ColivriDigitalTwin project offers the following key functionalities:

### Quick control of the laboratory

From a large format control panel, users can have a quick overview of all activities happening in the laboratory. This functionality enables:

- Intuitive control.
- Safety visualization.
- Functional simulation (audio).

### Show future configurations

The project also facilitates the display of future configurations, particularly on the screens, allowing:

- Intuitive control.
- Viewing from various platforms.

### AR - Adding information to the real world

In the context of Augmented Reality, the project enables users to view additional information about the equipment in the laboratory, including:

- Historical and real-time information viewing.
- Identification of points with critical information.
- Intuitive control.

## Getting Started

To get started with the project, please refer to the [official development documentation](https://imagine-uniandes.github.io/ColivriDigitalTwin/development/) for detailed information on the project structure, setup instructions, and development guidelines.

## Using the Space Mouse Compact Controller with the TVs Screen

To use the Space Mouse Compact Controller with the TV screens in the ColivriDigitalTwin project, follow these steps:

1. Download the drivers of the device from the official website: [3Dconnexion Drivers](https://3dconnexion.com/us/drivers/).
2. Download vJoy, a virtual joystick driver, from the following link: [vJoy on SourceForge](https://sourceforge.net/projects/vjoystick/).
3. Follow the instructions provided in the GitHub repository for the integration: [Sx2vJoy-test GitHub Repository](https://github.com/Lasse-B/Sx2vJoy-test).
4. Disable the 3Dconnexion KMJ Emulator to prevent conflicts with executions. You can find a guide on how to disable it in this [tutorial](https://wrw.is/how-to-disable-the-3dconnexion-kmj-emulator-game-controller-to-fix-conflicts/).
5. Configure the buttons on the 3Dconnexion controller as follows: set the left button as "M" and the right button as "Enter" for optimal functionality.

## Meta Quest 2 Integration

This section provides guidance on integrating the ColivriDigitalTwin project with Meta Quest 2 for an immersive experience.

###Prerequisites
Unity 20XX.XX or later.
Oculus Integration Package (version XX.XX.X).
Meta Quest 2 headset.

Installation
Clone this repository to your local machine.
Open the project in Unity.
Install the Oculus Integration package via the Unity Package Manager.
Configuring Oculus Integration
In Unity, navigate to Window > Package Manager.
Find "Oculus Integration" and install the latest version.
Follow the Oculus Integration documentation for configuring your project settings, including setting up the Oculus App ID.

Universal Render Pipeline (URP) Settings
Open the Graphics settings in Unity (Edit > Project Settings > Graphics).
Set the Scriptable Render Pipeline Settings to the URP Asset in your project.
Shadow Settings
Disable cast shadows to improve performance on the Meta Quest 2:

Navigate to the object that needs its shadows disabled.
In the Inspector window, uncheck "Cast Shadows."
Quality Settings
Adjust the quality settings to match the capabilities of the Meta Quest 2:

Open the Quality settings in Unity (Edit > Project Settings > Quality).
Create a new quality level specifically for Meta Quest 2.
Adjust settings such as anti-aliasing, texture quality, and anisotropic filtering to optimize performance.

## Development Team

The ColivriDigitalTwin project is being developed by a dedicated team, including:

<table>
  <tr>
    <td align="center" width="33%">
      <a href="https://github.com/pablo-figueroa-uniandes">
        <img src="https://github.com/pablo-figueroa-uniandes.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
    <td align="center" width="33%">
      <a href="https://github.com/VivianGomez">
        <img src="https://github.com/VivianGomez.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
    <td align="center" width="33%">
      <a href="https://github.com/Juanes1516">
        <img src="https://github.com/Juanes1516.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
  </tr>
  <tr>
    <td align="center">
      Pablo Figueroa Forero
    </td>
    <td align="center">
      Vivian Gómez Cubillos
    </td>
    <td align="center">
      Juan Esteban Rodríguez
    </td>
  </tr>
  <tr>
    <td align="center" width="33%">
      <a href="https://github.com/Mecon0710">
        <img src="https://github.com/Mecon0710.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
    <td align="center" width="33%">
      <a href="https://github.com/zejiran">
        <img src="https://github.com/zejiran.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
    <td align="center" width="33%">
      <a href="https://github.com/julian27m">
        <img src="https://github.com/julian27m.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
  </tr>
  <tr>
    <td align="center">
      Melissa Lizeth Contreras Rojas
    </td>
    <td align="center">
      Juan Sebastián Alegría Zúñiga
    </td>
    <td align="center">
      Julian Camilo Mora Valbuena
    </td>
  </tr>
  <tr>
    <td align="center" width="33%">
      <a href="https://github.com/nFei">
        <img src="https://github.com/nFei.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
    <td align="center" width="33%">
      <a href="https://github.com/Valentina1125">
        <img src="https://github.com/Valentina1125.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
    <td align="center" width="33%">
      <a href="https://github.com/abelarismendy">
        <img src="https://github.com/abelarismendy.png" width="100" height="100" style="border-radius:50%">
      </a>
    </td>
  </tr>
  <tr>
    <td align="center">
      Nicolas Falla Bernal
    </td>
    <td align="center">
      Valentina Uribe
    </td>
    <td align="center">
      Abel Arismendy
    </td>
  </tr>
</table>

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

- **[MIT license](LICENSE)**
- Copyright 2023 © Grupo Imagine Uniandes
