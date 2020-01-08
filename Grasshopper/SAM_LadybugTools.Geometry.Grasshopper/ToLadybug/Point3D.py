# coding=utf-8
"""Base class for all 1D geometries in 3D space (Ray3D and LineSegment3D)."""
from __future__ import division

from .pointvector import Point3D  as LB_Point3D
from SAM.Geometry.Spatial import Point3D as SAM_Point3D

def Convert(lb_Point3D):
	sam_Point3D = SAM_Point3D(lb_Point3D.x, lb_Point3D.y, lb_Point3D.z)
	return sam_Point3D



from .pointvector import Point3D  as LB_Point3D
from SAM.Geometry.Spatial import Point3D as SAM_Point3D

def Convert(lb_Point3D):
	sam_Point3D = SAM_Point3D(lb_Point3D.x, lb_Point3D.y, lb_Point3D.z)
	return sam_Point3D


from .pointvector import Point3D

a = Point3D(x, y, z)

