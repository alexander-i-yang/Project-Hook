<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.10.2" name="Mechanics" tilewidth="24" tileheight="24" tilecount="10" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="0" type="Blue Crystal">
  <image width="8" height="8" source="../../Sprites/Mechanics/Crystal Blue.png"/>
 </tile>
 <tile id="1" type="Moving Box">
  <image width="24" height="24" source="../../Sprites/Mechanics/Moving Box.png"/>
 </tile>
 <tile id="3" type="Spawn">
  <image width="8" height="12" source="../../Sprites/Mechanics/Spawn.png"/>
 </tile>
 <tile id="5" type="Gate">
  <image width="24" height="8" source="../../Sprites/Mechanics/Temp_Gate.png"/>
 </tile>
 <tile id="6" type="Firefly">
  <properties>
   <property name="Next" type="object" value="0"/>
  </properties>
  <image width="8" height="8" source="../../Sprites/Mechanics/Crystal Yellow.png"/>
 </tile>
 <tile id="7" type="ZiplineEndpoint">
  <image width="8" height="8" source="../../Sprites/Mechanics/Firefly End.png"/>
 </tile>
 <tile id="8" type="Waterfall">
  <image width="8" height="8" source="../../Sprites/Mechanics/Waterfall.png"/>
 </tile>
 <tile id="10" x="2" y="7" width="9" height="17" type="Chair">
  <image width="34" height="24" source="../../Sprites/Placeholder/Chair.png"/>
 </tile>
 <tile id="11" type="Robot Lame">
  <image width="13" height="15" source="../../Sprites/Enemies/Robot.png"/>
 </tile>
 <tile id="12" type="Zipline">
  <properties>
   <property name="SetEndpoint" type="object" value="0"/>
   <property name="Speed" type="int" value="100"/>
  </properties>
  <image width="24" height="24" source="../../Sprites/Tilesets/Breakable.png"/>
 </tile>
</tileset>
