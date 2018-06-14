<?xml version="1.0" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
	<html>
	<body>
	<h2>Escola</h2>
	<hr></hr>
	<table border="1">
		<tr bgcolor="gray">
	        	<th align="center">Turma</th>
	        	<th align="center">Area</th>
	        	<th align="center">Aluno</th>
	        	<th align="center">Idade</th>
	        	<th align="center">Sexo</th>
		</tr>
		<xsl:for-each select="escola/turma/aluno">
		<tr>
		<td align="center">
		<xsl:value-of select="../@ano"/>
		<xsl:value-of select="..@letra"/>
		</td>
		<td align="center">
		<xsl:value-of select="..@area"/>
		</td>
		<td align="center">
		<xsl:value-of select="@nome"/>
		</td>
		<td align="center">
		<xsl:value-of select="@idade"/>
		</td>
		<td align="center">
		<xsl:value-of select="@sexo"/>
		</td>
		</tr>
		</xsl:for-each>
	</table>
	<hr></hr>
	<p>
	<b>
		<xsl:text>Total de Turmas: </xsl:text>
	</b>
		<xsl:value-of select="count(escola/turma)"/>
	</br>

	<b>
		<xsl:text>Total de Alunos: </xsl:text>
	</b>
		<xsl:value-of select="count(escola/turma/aluno)"/>
	</br>
	</p>
	</body>
	</html>
</xsl:template>
</xsl:stylesheet>