<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:template match="todolist">

<HTML>

<BODY>

<H1>
<b><i>TODOLIST</i></b>
</H1>
	
<TABLE BORDER="2">

<THEAD>

<TR>

<TH>

<b>Title</b>

</TH>

<TH>

<b>Progress</b>

</TH>

<TH>

<b>Description</b>

</TH>

<TH>

<b>Implementer</b>

</TH>

<TH>

<b>Days left</b>

</TH>

</TR>

</THEAD>

<TBODY>

<xsl:for-each select="//todo">

<TR>

<TD>

<b><xsl:value-of select="./title" /></b>

</TD>

<TD>

<b><xsl:value-of select="./progress" /></b>

</TD>

<TD>

<b><xsl:value-of select="./description" /></b>

</TD>

<TD>

<b><xsl:value-of select="./implementer" /></b>

</TD>

<TD>

<b><xsl:value-of select="./daysLeft" /></b>


</TD>

</TR>

</xsl:for-each>

</TBODY>

</TABLE>

</BODY>

</HTML>

</xsl:template>
</xsl:stylesheet>