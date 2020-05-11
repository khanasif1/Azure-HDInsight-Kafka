# Azure-HDInsight Kafka
This repository has sample application which has HD Insight Kafka.
![Image description](https://i.ytimg.com/vi/mPONv2Ye5qk/maxresdefault.jpg)
# What is Apache Kafka in Azure HDInsight
Apache Kafka is an open-source distributed streaming platform that can be used to build real-time streaming data pipelines and applications. Kafka also provides message broker functionality similar to a message queue, where you can publish and subscribe to named data streams.

The following are specific characteristics of Kafka on HDInsight:
<ul>
<li>
It's a managed service that provides a simplified configuration process. The result is a configuration that is tested and supported by Microsoft.
</li>
<li>
Microsoft provides a 99.9% Service Level Agreement (SLA) on Kafka uptime. For more information, see the SLA information for HDInsight document.
</li>
<li>
It uses Azure Managed Disks as the backing store for Kafka. Managed Disks can provide up to 16 TB of storage per Kafka broker. For information on configuring managed disks with Kafka on HDInsight, see Increase scalability of Apache Kafka on HDInsight.
</li>
<li>
Kafka was designed with a single dimensional view of a rack. Azure separates a rack into two dimensions - Update Domains (UD) and Fault Domains (FD). Microsoft provides tools that rebalance Kafka partitions and replicas across UDs and FDs.
</li>
<li>
HDInsight allows you to change the number of worker nodes (which host the Kafka-broker) after cluster creation. Scaling can be performed from the Azure portal, Azure PowerShell, and other Azure management interfaces. For Kafka, you should rebalance partition replicas after scaling operations. Rebalancing partitions allows Kafka to take advantage of the new number of worker nodes.
</li>
<li>
Azure Monitor logs can be used to monitor Kafka on HDInsight. Azure Monitor logs surfaces virtual machine level information, such as disk and NIC metrics, and JMX metrics from Kafka.
</li></ul>
