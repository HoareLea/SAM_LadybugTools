[![Build (Windows)](https://github.com/SAM-BIM/SAM_LadybugTools/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/SAM-BIM/SAM_LadybugTools/actions/workflows/build.yml)
[![Installer (latest)](https://img.shields.io/github/v/release/SAM-BIM/SAM_Deploy?label=installer)](https://github.com/SAM-BIM/SAM_Deploy/releases/latest)

# SAM_LadybugTools

<a href="https://github.com/SAM-BIM/SAM">
  <img src="https://github.com/SAM-BIM/SAM/blob/master/Grasshopper/SAM.Core.Grasshopper/Resources/SAM_Small.png"
       align="left" hspace="10" vspace="6">
</a>

**SAM_LadybugTools** is part of the **SAM (Sustainable Analytical Model) Toolkit** —  
an open-source collection of tools designed to help engineers create, manage,
and process analytical building models for energy and environmental analysis.

This repository provides **bi-directional integration between SAM and Ladybug Tools**,
enabling analytical models and data to be exchanged between SAM workflows
and the Ladybug Tools ecosystem.

The integration aligns SAM analytical objects with the **Honeybee model schema**,
allowing models to be translated, mapped, and synchronised
between SAM and Ladybug Tools–based workflows.

Welcome — and let’s keep the open-source journey going. 🤝

---

## Features

- Bi-directional model exchange between SAM and Ladybug Tools
- Mapping of SAM analytical objects to Honeybee schema entities
- Support for Honeybee-based environmental and energy workflows
- Integration with Grasshopper-based visual programming environments

---

## References

- 🌿 **Ladybug Tools:** https://www.ladybug.tools/  
- 🐝 **Honeybee Model Schema:**  
  https://www.ladybug.tools/honeybee-schema/model.html  

---

## Resources
- 📘 **SAM Wiki:** https://github.com/SAM-BIM/SAM/wiki  
- 🧠 **SAM Core:** https://github.com/SAM-BIM/SAM  

---

## Installing

To install **SAM** using the Windows installer, download and run the  
[latest installer](https://github.com/SAM-BIM/SAM_Deploy/releases/latest).

Alternatively, you can build the toolkit from source using Visual Studio.  
See the main repository for details:  
👉 https://github.com/SAM-BIM/SAM

---

## Development notes

- Target framework: **.NET / C#**
- Object mapping follows SAM-BIM analytical modelling conventions
- Honeybee schema compatibility is maintained where applicable
- New or modified `.cs` files must include the SPDX header from `COPYRIGHT_HEADER.txt`

---

## Licence

This repository is free software licensed under the  
**GNU Lesser General Public License v3.0 or later (LGPL-3.0-or-later)**.

Each contributor retains copyright to their respective contributions.  
The project history (Git) records authorship and provenance of all changes.

See:
- `LICENSE`
- `NOTICE`
- `COPYRIGHT_HEADER.txt`
