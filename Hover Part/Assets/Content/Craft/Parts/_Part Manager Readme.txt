The part manager's inspector window contains several utilities for creating part related assets as well as showing a list of parts that have been added to the project so far.

Parts will not be included in the mod unless they are 'saved' and show up in the mod builder window's list of parts. Parts can be saved by clicking the save button next to the part name in the part manager or with the save button on the root game object (PartDefinition script) of a part prefab. You can also quickly save all parts in the project with the 'Save All Parts' button in the PartManger.

A part prefab consists of a lot of data related to the part as well as the prefab that is used when the part is loaded in the game. When a part is saved, XML is generated representing the part and the part's prefab asset that is loaded at runtime is saved. The XML and prefab are both saved in the resources directory and should not be manually edited. When editing parts, do so by editing the part prefab in the Assets/Content/Craft/Parts/Prefabs/ directory and using one of the 'Save Part' buttons mentioned above.


A part prefab consists of the following objects:

   - Root Game Object
      - The root game object should have a PartDefinition script. The inspector for this script can be used to save the part as well as to easily add part modifiers, designer parts, and attach points to the part.

   - PartType object
      - The part type object contains a lot of data that describes the part, including the part modifiers. It should be a prefab instance with no overrides. The part type script contains some configuration information common to all parts of this type. The designer part script contains information related to how the part shows up in the part list in the designer. Modifier editor scripts can be added to the PartType to add various functionality to the part and the default values for those modifiers can be set here. Note that all parts should have a 'Config' modifier which contains a lot of common configuration data for all parts.

   - DesignerParts object
      - This contains child objects that are prefab instances of the PartType prefab and they may have prefab overrides. Each designer part object represents a new part in the part list in the designer. This allows you to have multiple parts that are all variations of the same part.

   - Part Styles object
      - This object has a PartDefinitionStyles script that contains some part style related options and the list of 'subparts' for the part. Subparts are used as a way to use different texturing on different pieces of the same part. New subparts can be created with this script, with style configuration for each one appearing as child objects of the PartStyles object. Each subpart can contain a list of part styles (typically used for changing the mesh of a part). Each part style can then contain a list of texture styles that are allowed for that part style. New texture styles can be created via the PartManager.

   - Prefab object
      - The prefab object will be saved as the root object of a prefab that is loaded at runtime to represent the part. The one exception to this is if the prefab object has a PartDefinitionPrefabReferenceEditor script attached, which can be used to point to a stock prefab that already exists in the game. PartColliderScripts should be added to prefab objects with colliders, with one being specified as the primary collider. A PartMeshScript can be added to objects with a MeshRenderer to tweak some rendering related options. Part modifiers scripts will be automatically added to the part's root game object at runtime and should not be manually added to the prefab.

   - AttachPoints object
      - Each child object (with an AttachPointEditorScript) should represent an attach point for the part. These child objects can be moved and rotated into proper position for the attach point.


Part modifiers consist of a data object and a monobehavior. Part modifiers are used to add a specific set of functionality to a part. Many part modifiers are written to provide the functionality of a specific part, but others provide common functionality can can be added to multiple parts.

   - PartModifierData
      - Represents the state of the part and is used for saving/loading the part.
      - Use attributes from the 'ModApi.Craft.Parts.Attributes' namespace on fields to get them to show up in the part properties flyout in the designer and/or to control their serialization for part XML.
      - Override OnDesignerInitialization and use the IDesignerPartPropertiesModifierInterface to hook into various events related to the part properties (such as OnPropertyChanged).

   - PartModifierScript
      - Contains the logic of the part such as the update methods.
      - The 'Data' property can be used to access the associated PartModifierData, generally used to read or write values to/from the data object.
      - Implement interfaces in the 'ModApi.GameLoop.Interfaces' namespace to implement Update, FixedUpdate, LateUpdate and other update related methods. This is recommended over the standard Unity update methods for part modifier scripts.
