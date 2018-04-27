<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" omit-xml-declaration="yes" indent="yes"/>
	<xsl:template match="/">
\documentclass[letterpaper,10pt,twoside,twocolumn,openany]{book}
\usepackage[english]{babel}
\usepackage[utf8]{inputenc}
\usepackage{lipsum}
\usepackage{listings}
\usepackage{soul}
\usepackage{dnd}

\lstset{%
  basicstyle=\ttfamily,
  language=[LaTeX]{TeX},
}

\begin{document}

\header{Lieux}
\begin{dndtable}[XX]
   	\textbf{FR} <xsl:text disable-output-escaping="yes"><![CDATA[&]]></xsl:text> \textbf{EN} \\
      <xsl:for-each select="terms/term">
	  <xsl:sort data-type="text" order="ascending" select="@fr"/>
        <xsl:value-of select="@fr"/> <xsl:text disable-output-escaping="yes"><![CDATA[&]]></xsl:text> <xsl:value-of select="@en"/> \\
      </xsl:for-each>
\end{dndtable}

\header{Places}
\begin{dndtable}[XX]
   	\textbf{EN} <xsl:text disable-output-escaping="yes"><![CDATA[&]]></xsl:text> \textbf{FR} \\
      <xsl:for-each select="terms/term">
	  <xsl:sort data-type="text" order="ascending" select="@en"/>
        <xsl:value-of select="@en"/> <xsl:text disable-output-escaping="yes"><![CDATA[&]]></xsl:text> <xsl:value-of select="@fr"/> \\
      </xsl:for-each>
\end{dndtable}

\end{document}
	</xsl:template>
</xsl:stylesheet>