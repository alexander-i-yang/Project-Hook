<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.10.1" name="Spikes" tilewidth="8" tileheight="8" tilecount="16" columns="4">
 <image source="../../Sprites/Tilesets/Spikes.png" width="32" height="32"/>
 <tile id="0" type="SpikeUp">
  <objectgroup draworder="index" id="3">
   <object id="5" x="1" y="6" width="6" height="2"/>
  </objectgroup>
 </tile>
 <tile id="1" type="SpikeLeft">
  <objectgroup draworder="index" id="2">
   <object id="1" x="4" y="1" width="4" height="6"/>
  </objectgroup>
 </tile>
 <tile id="2" type="SpikeDown">
  <objectgroup draworder="index" id="2">
   <object id="1" x="1" y="0" width="6" height="2"/>
  </objectgroup>
 </tile>
 <tile id="3" type="SpikeRight">
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="1" width="2" height="6"/>
  </objectgroup>
 </tile>
 <tile id="9" type="DirectionalSpikeUp">
  <properties>
   <property name="SetDirectionFromCode" type="int" value="1"/>
  </properties>
 </tile>
 <tile id="10" type="ConnectedSpike">
  <properties>
   <property name="AttachedTo" type="object" value="0"/>
   <property name="SetDirectionFromCode" type="int" value="1"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" type="ConnectedSpike" x="0" y="7" width="8" height="1"/>
  </objectgroup>
 </tile>
 <tile id="11" type="ConnectedSpike">
  <properties>
   <property name="AttachedTo" type="object" value="0"/>
   <property name="SetDirectionFromCode" type="int" value="2"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="7" y="0" width="1" height="8"/>
  </objectgroup>
 </tile>
 <tile id="14" type="ConnectedSpike">
  <properties>
   <property name="AttachedTo" type="object" value="0"/>
   <property name="SetDirectionFromCode" type="int" value="0"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="1" height="8"/>
  </objectgroup>
 </tile>
 <tile id="15" type="ConnectedSpike">
  <properties>
   <property name="AttachedTo" type="object" value="0"/>
   <property name="SetDirectionFromCode" type="int" value="3"/>
  </properties>
  <objectgroup draworder="index" id="2">
   <object id="1" x="0" y="0" width="8" height="1"/>
  </objectgroup>
 </tile>
</tileset>
