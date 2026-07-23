# Executables paths passed to test scripts
THIS_DIR = $(top_srcdir)/test
EXEPATH = ../bin
LISTGEOEXE = $(EXEPATH)/listgeo
GEOTIFCPEXE = $(EXEPATH)/geotifcp

# Test scripts
TESTLISTGEO = $(THIS_DIR)/testlistgeo
TESTGEOTIFCP = $(THIS_DIR)/testgeotifcp

EXTRA_DIST = testlistgeo testlistgeo_out.dist \
	     testgeotifcp testgeotifcp_out.dist \
             data/byte.tif \
             data/byte_v11.tif \
             data/pcs_32064.tif \
             data/GeogPrimeMeridianGeoKey.tif \
             data/GeogGeodeticDatumGeoKey.tif \
             data/GeogAngularUnitsGeoKey_9114.tif \
             data/ProjLinearUnitsGeoKey_9036.tif \
             data/ProjectedCSTypeGeoKey_28191_cassini_soldner.tif \
             data/cassini_soldner.tif \
             data/ProjectedCSTypeGeoKey_27200_new_zealand_mapping_grid.tif \
             data/new_zealand_mapping_grid.tif \
             data/ProjectedCSTypeGeoKey_29101_polyconic.tif \
             data/polyconic.tif \
             data/ProjectedCSTypeGeoKey_6808_hotine_oblique_mercator_variant_a.tif \
             data/hotine_oblique_mercator_variant_a.tif \
             data/ProjectedCSTypeGeoKey_8065_hotine_oblique_mercator_variant_b.tif \
             data/hotine_oblique_mercator_variant_b.tif \
             data/ProjectedCSTypeGeoKey_8441_oblique_mercator_laborde.tif \
             data/oblique_mercator_laborde.tif \
             data/ProjectedCSTypeGeoKey_5456_lcc1sp.tif \
             data/lcc1sp.tif \
             data/ProjectedCSTypeGeoKey_5329_mercator1sp.tif \
             data/mercator1sp.tif \
             data/ProjectedCSTypeGeoKey_5641_mercator2sp.tif \
             data/mercator2sp.tif \
             data/ProjectedCSTypeGeoKey_5588_oblique_stereographic.tif \
             data/oblique_stereographic.tif \
             data/ProjectedCSTypeGeoKey_5482_polar_stereographic_variant_a.tif \
             data/polar_stereographic_variant_a.tif \
             data/ProjectedCSTypeGeoKey_3032_polar_stereographic_variant_b.tif \
             data/polar_stereographic_variant_b.tif \
             data/ProjectedCSTypeGeoKey_3814_transverse_mercator.tif \
             data/transverse_mercator.tif \
             data/ProjectedCSTypeGeoKey_2046_transverse_mercator_south_oriented.tif \
             data/transverse_mercator_south_oriented.tif \
             data/ProjectedCSTypeGeoKey_3812_lcc2sp.tif \
             data/lcc2sp.tif \
             data/ProjectedCSTypeGeoKey_3083_albers_equal_area.tif \
             data/albers_equal_area.tif \
             data/ProjectedCSTypeGeoKey_3035_lambert_azimuthal_equal_area.tif \
             data/lambert_azimuthal_equal_area.tif \
             data/ProjectedCSTypeGeoKey_3410_lambert_cylindrical_equal_area.tif \
             data/lambert_cylindrical_equal_area.tif \
             data/ProjectedCSTypeGeoKey_4087_equidistant_cylindrical.tif \
             data/equidistant_cylindrical.tif \
             data/pixel_is_point_wgs84.tif \
             data/epsg_27563_only_pcs_code.tif \
             data/epsg_27563_allgeokeys.tif

check-local:
	$(TESTLISTGEO) $(LISTGEOEXE)
	$(TESTGEOTIFCP) $(GEOTIFCPEXE) $(LISTGEOEXE)

